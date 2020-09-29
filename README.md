# Messaging workshop

This repository contains the exercises for the messaging training.

If you have any difficulty preparing your machine, or following this document, please raise an issue in this repository ASAP so that we can resolve the problem before the workshop begins.

## Preparing your machine for the workshop

- [Install the pre-requisites](#install-the-pre-requisites)
- [Get a copy of this repository](#get-a-copy-of-this-repository)
- [Build the exercise solutions](#build-the-exercise-solutions)

### Install the pre-requisites

To complete the exercises, you require a Windows machine and Visual Studio.

#### Visual Studio

Install [Visual Studio 2019](https://www.visualstudio.com) (Community, Professional, or Enterprise) with the following workloads:
  - .NET desktop development

### Get a copy of this repository

Clone or download this repo. If you're downloading a zip copy of the repo, ensure the zip file is unblocked before decompressing it:

* Right-click on the downloaded copy
* Click "Properties"
* On the "General" properties page, check the "Unblock" checkbox
* Click "OK"

### Build the exercise solutions

The exercises are contained in Visual Studio solutions under [exercises](exercises). All the solutions require NuGet package restore. This may be possible at the workshop venue (you can verify with the workshop organizers if internet access is available at the venue) but to ensure you can build the solutions during the workshop, we recommend you restore all NuGet packages and build all the solutions before the workshop starts.

## Troubleshooting

### Access denied exception with learning transport

If you are getting access denied exceptions on the file system while running the exercises the cause is likely virusscanners, backup or file synchronisation software like dropbox or onedrive opening these files. Exclude the `.learningtransport` folder or temporarily disable these tools.

## FAQ

If the answer to your question is not listed here, consult your on-site trainer.
