using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using CoviIDApiCore.Models.Database;
using CoviIDApiCore.V1.Constants;
using CoviIDApiCore.V1.DTOs.Organisation;
using CoviIDApiCore.V1.DTOs.System;
using CoviIDApiCore.V1.Interfaces.Repositories;
using CoviIDApiCore.V1.Interfaces.Services;

using Newtonsoft.Json;

namespace CoviIDApiCore.V1.Services
{
    public class OrganisationService : IOrganisationService
    {
        private readonly IOrganisationRepository _organisationRepository;
        private readonly IOrganisationCounterRepository _organisationCounterRepository;

        public OrganisationService(IOrganisationRepository organisationRepository, IOrganisationCounterRepository organisationCounterRepository)
        {
            _organisationRepository = organisationRepository;
            _organisationCounterRepository = organisationCounterRepository;
        }

        public async Task CreateAsync(CreateOrganisationRequest payload)
        {
            var companyNameRef = payload.FormResponse.Definition.Fields
                .FirstOrDefault(t => t.Title == ParameterConstants.CompanyName)?
                .Reference;

            var organisation = new Organisation()
            {
                Name = payload.FormResponse.Answers
                    .FirstOrDefault(t => string.Equals(t.Field.Reference, companyNameRef, StringComparison.Ordinal))?
                    .Text,
                Payload = JsonConvert.SerializeObject(payload),
                CreatedAt = DateTime.UtcNow
            };

            await _organisationRepository.AddAsync(organisation);

            await _organisationRepository.SaveAsync();
        }

        public async Task<Response> GetAsync(string id)
        {
            var organisation = await _organisationRepository.GetAsync(Guid.Parse(id));

            return organisation == default
                ? new Response(false, HttpStatusCode.NotFound,Messages.Org_NotExists)
                : new Response(organisation, HttpStatusCode.OK);
        }

        public async Task<Response> UpdateCountAsync(string id, UpdateCountRequest payload)
        {
            var balance = 0;

            var organisation = await _organisationRepository.GetAsync(Guid.Parse(id));

            if (organisation == default)
                return new Response(false, HttpStatusCode.NotFound, Messages.Org_NotExists);

            var lastCount = await _organisationCounterRepository.GetLastByOrganisation(organisation);

            balance = lastCount?.Balance ?? 0;

            var newCount = new OrganisationCounter()
            {
                Organisation = organisation,
                Date = DateTime.UtcNow,
                Movement = payload.Movement,
                Balance = balance + payload.Movement,
                DeviceIdentifier = payload.DeviceIdentifier
            };

            await _organisationCounterRepository.AddAsync(newCount);

            await _organisationCounterRepository.SaveAsync();

            return new Response();
        }
    }
}