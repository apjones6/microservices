# Microservices API Proxy #

This fairly simple project is a working progress proof of concept to demonstrate
creating proxies to remote services to aggregate data. The idea is to simply
define interfaces and types, with a combination of attributes and configuration
data. The interfaces can then be dynamically implemented to handle the remote
requests to endpoints, data conversion, authorization, and any other required
repetitive tasks.

## Projects ##

### Microservices.Core ###

This package contains the reusable code for creating proxies. It depends on
Castle.Core DynamicProxy to implement interfaces. It exposes configuration
classes and builders, and anything else required for common or extensible proxy
creation.

### Microservices.Orders & Microservices.Products ###

These are example projects, using Microservices.Core to handle proxy generation.
They have no direct dependencies on each other, instead implementing their own
versions of models.

# NuGet Packages #

Currently this project uses NuGet packages which are not included in the GIT repository. Building the solution in Visual Studio should automatically install any missing packages, using the package restore feature.

# Git Flow #

This project uses the development model set out in http://nvie.com/posts/a-successful-git-branching-model. The conventions set out in this article, such as develop and master branches, and naming release-\*, and so on are followed as written.

# Version History #

There haven't been any releases yet!
