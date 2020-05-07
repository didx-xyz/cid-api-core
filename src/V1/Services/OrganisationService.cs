﻿using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CoviIDApiCore.Exceptions;
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
        private readonly IEmailService _emailService;
        private readonly IQRCodeService _qrCodeService;

        public OrganisationService(IOrganisationRepository organisationRepository, IOrganisationCounterRepository organisationCounterRepository, IEmailService emailService, IQRCodeService qrCodeService)
        {
            _organisationRepository = organisationRepository;
            _organisationCounterRepository = organisationCounterRepository;
            _emailService = emailService;
            _qrCodeService = qrCodeService;
        }

        public async Task CreateAsync(CreateOrganisationRequest payload)
        {
            var companyNameRef = payload.FormResponse.Definition.Fields
                .FirstOrDefault(t => string.Equals(t.Title, DefinitionConstants.CompanyName, StringComparison.Ordinal))?
                .Reference;

            var companyName = payload.FormResponse.Answers
                .FirstOrDefault(t => string.Equals(t.Field.Reference, companyNameRef, StringComparison.Ordinal))?
                .Text;

            var organisation = new Organisation()
            {
                Name = companyName,
                Payload = JsonConvert.SerializeObject(payload),
                CreatedAt = DateTime.UtcNow
            };

            await _organisationRepository.AddAsync(organisation);

            await _organisationRepository.SaveAsync();

            await NotifyOrganisation(companyName, payload, organisation);
        }

        public async Task<Response> GetAsync(string id)
        {
            var organisation = await _organisationRepository.GetAsync(Guid.Parse(id));

            if (organisation == default)
                return new Response(false, HttpStatusCode.NotFound, Messages.Org_NotExists);

            var orgCounter = await _organisationCounterRepository.GetLastByOrganisation(organisation);

            var totalScans = _organisationCounterRepository.Count();

            return new Response(new OrganisationDTO(organisation, orgCounter, totalScans), HttpStatusCode.OK);
        }

        public async Task UpdateCountAsync(string id, string deviceId, UpdateType updateType)
        {
            var balance = 0;

            var organisation = await _organisationRepository.GetAsync(Guid.Parse(id));

            if (organisation == default)
                throw new NotFoundException(Messages.Org_NotExists);

            var lastCount = await _organisationCounterRepository.GetLastByOrganisation(organisation);

            balance = lastCount?.Balance ?? 0;

            if (balance < 1 && updateType == UpdateType.Subtraction)
                throw new ValidationException(Messages.Org_NegBalance);

            var newCount = new OrganisationCounter()
            {
                Organisation = organisation,
                Date = DateTime.UtcNow,
                Movement = updateType == UpdateType.Addition
                    ? 1
                    : -1,
                Balance = updateType == UpdateType.Addition
                    ? balance + 1
                    : balance - 1
                ,
                DeviceIdentifier = deviceId
            };

            await _organisationCounterRepository.AddAsync(newCount);

            await _organisationCounterRepository.SaveAsync();
        }

        private async Task NotifyOrganisation(string companyName, CreateOrganisationRequest payload, Organisation organisation)
        {
            var emailAddressRef = payload.FormResponse.Definition.Fields
                .FirstOrDefault(t => string.Equals(t.Title, DefinitionConstants.EmailAdress, StringComparison.Ordinal))?
                .Reference;

            var emailAddress = payload.FormResponse.Answers
                .FirstOrDefault(t => string.Equals(t.Field.Reference, emailAddressRef, StringComparison.Ordinal))?
                .Email;

            if (string.IsNullOrEmpty(emailAddress))
                throw new ValidationException(Messages.Org_EmailEmpty);

            //TODO: Queueing
            await _emailService.SendEmail(
                emailAddress,
                companyName,
                _qrCodeService.GenerateQRCode(organisation.Id.ToString()),
                DefinitionConstants.EmailTemplates.OrganisationWelcome);
        }
    }
}