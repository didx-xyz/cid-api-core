<div align="center">
    <img src="./imgs/logo-dark.png">
</div>
<h3>
    Covi-ID is an open source risk management tool designed to protect privacy.
</h3>

---

# API Core

# Index

#### [Introduction to the Covi ID Application](#introduction-to-the-covi-id-application)
#### [How the app works](#how-the-app-works)
* [Privacy Preserving Technology at the Core of the Application’s Design](#privacy-preserving-technology-at-the-core-of-the-applications-design)
* [Four Key Functionalities of the Web Application](#four-key-functionalities-of-the-web-application)
* [Interoperability](#interoperability)
#### [Project Details](#project-details)
* [Getting started](#getting-started)
* [Learn More About `.NET`](#learn-more-about-net)


This repository is for the web app of [Covi-ID](https://coviid.me/). This product was designed with data privacy and privacy-preserving considerations at the core of its architecture. This was done by permitting the creation of Covi-ID wallets in the form of self-sovereign identities. This is facilitated by employing StreetCred, the custodian wallet holder. In essence, this will allow individuals to prove you do not pose a public health risk. 

The other related repositories can be found here:
#### [> `Mobile App`](https://github.com/covi-id/cid-mob-app)
#### [> `Web App`](https://github.com/covi-id/cid-web-app)

---

# Introduction to the Covi ID Application 

The Covi-ID application offers a mobile app that allows the health status of individuals to be determined before entering public spaces. Currently, the app is used from a verifiers perspective in that they can verify the health status of individuals before those individuals enter their locations/premises. This is achieved through scanning QR Codes. For an individual to generate their individual QR Code, this will need to be done through the web platform. The Covi-ID application is, therefore, a two-fold project. There exists a web application and a mobile application. The web application is where the Covi-ID’s are created and the QR Codes are generated (either on an individual or organisational level) and the mobile application facilitates the scanning of the QR Codes to determine health status and organisational count. In essence, the app is more of a QR code scanner than an interactive application.

Verifiers can be classified as places/locations whereby the public may gather such as airports, shops, restaurants, schools, and more. Therefore, the initial success of the mobile application is dependent on verifiers adopting it as well as users generating their Covi-ID’s via the web application. A users health status will be presented via one of three colours: green (immune), amber (apply social distancing) and red (self-isolate). The unique selling point of this version of the application is the ability for organisational verifiers to have a real-time count of the individuals inside their building at a given point in time, made possible. through **Organisation Identifier QR Codes**. The process works by the verifier scanning the organisation identifier QR Code of their specific organisation. This will mean that the verifier's Covi-ID app has now ‘logged in’ to the account of, Old Mutual CT, for example. As employees arrive for work, the verifier will scan their QR Code and the ‘organisational count’ for Old Mutual will go up by 1, and so on. This will work even if there are multiple entrances at an organisations buildings because of the **Organisations Identifier QR Code** being the common and unique ID.

---

# How the app works 

## Privacy Preserving Technology at the Core of the Applications Design

The creation of all Covi-ID’s will be facilitated by the self-sovereign identification management system, Streetcred. Currently, Streetcred acts as a custodian as it holds all of the created wallets generated by users. This will be the case until later versions of the mobile application are developed, whereby an SSI wallet app will be created. In that case, users with a cellphone will be able to import their wallet and all of its metadata directly to their mobile device whereby they will become the only holders of this information. Once exported from Streetcred, it will be deleted off their database. Should a user not have a mobile device, their custodial wallet can be generated for them through a trusted third party acquaintance and their wallet data can be stored through a friend or family member with a mobile device whom they give access to.

<img src="./imgs/Issuer-holder-verifier.png">

## Four Key Functionalities of the Web Application

* An individual user is able to input their details and generate their Covi-ID and subsequent QR Code. This can be downloaded and stored as an image or even printed and stored in paper-based form. 
*An individual user will be able to input test details if they have gone for tests. These details will be stored in the Covi-ID database and once collaboration with the testing facilities is in place, these results will be validated and verified and QR Code status will be updated accordingly. 
*All individual Covi-ID’s will be stored by a self-sovereign identity custodial wallet holder, Streetcred. 
*An organisation is able to register as an organisation verifier whereby an organisation identifier QR Code will be generated. This can be used to track the number of people in the organisation at all times. 

<img src="./imgs/coviid-trust.png">

## Interoperability

This product was ultimately designed to give the user control over their data. Our system is designed to give as much control back to the user, allowing the user to claim their SSI and be in control of the information which they give out. This will be truly achieved when the SSI Wallet is accessible from the user’s mobile device. If the user is unable to claim their identity, a trusted set up is created to hold their wallet in a custodial manner. 

<img src="./imgs/agent-to-agent.png">

---

# Project Details

## Getting Started

In the project directory:

### 1. App Settings

Make a file called `appsettings.json`

Copy the content out of the `appsettings.example.json` and fill in the needed information

### 2. Get `.NET` up

Download the latest `.NET` Core SDK
* [`.NET` Core 2.2 SDK](https://github.com/dotnet/core/blob/master/release-notes/2.2/README.md)

`.NET` Core Releases and Daily Builds
* [.NET Core released builds](https://github.com/dotnet/core/blob/master/release-notes/README.md)
* [.NET Core daily builds](https://github.com/dotnet/core/blob/master/daily-builds.md)

`.NET` Commands

#### Restore
The dotnet restore command restores any packages or dependencies that the project requires. See [here](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-restore?tabs=netcore2x) for details on the command.

#### Build
The dotnet build command builds the project along with all of its dependencies. See [here](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-build) for more details.

#### Run
This is the last command that needs to be performed. It executes the code so that the code starts running. See [here](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-run?tabs=netcore30) for more information.

### Learn More About `.NET`

[Learn about .NET Core](https://docs.microsoft.com/dotnet/core)

In order to run the code certain commands need to be executed. This can be done through the `.Net CLI`. This [link](https://docs.microsoft.com/en-us/dotnet/core/tools/?tabs=netcore2x) contains general information on the dotnet CLI commands, and this [link](https://dzone.com/articles/create-and-run-a-net-core-application-using-cli-to) explains the flows required to run a dotnet project from the CLI. 