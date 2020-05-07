﻿using CoviIDApiCore.V1.DTOs.Credentials;
using System.Threading.Tasks;

namespace CoviIDApiCore.V1.Interfaces.Services
{
    public interface ICredentialService
    {
        Task<CredentialsContract> CreatePerson(string connectionId, PersonCredentialParameters personalDetials);
        Task<CredentialsContract> CreateCovidTest(string connectionId, CovidTestCredentialParameters covidTestCredential, string walletId);
        Task CreatePersonAndCovidTestCredentials(CovidTestCredentialParameters covidTest, PersonCredentialParameters person, string walletId);
        Task<CoviIDCredentialContract> GetCoviIDCredentials(string walletId);
    }
}
