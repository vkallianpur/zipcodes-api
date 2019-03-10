# README

The Zipcode Management REST API allows you to query zip code details from publicly available HUD data.
The REST API provides these endpoints to query zip code details as well as to import the HUD data files into the system:
  GET /api/hud/zipcodes/{zip}
  POST /api/hud/mappings/zipToCbsa
  POST /api/hud/mappings/cbsaToMsa

Versions:
The API is built using .NET 4.6, Visual Studio 2017, ASP.NET MVC Web API 2, C#, Mongo DB 4.0.6

Dependencies:
The API uses Mongo DB to store the HUD data imported into the system.
The API expects the HUD data to be imported using csv files which need to have a header row as the first row and these required headers:
-The 'Zip to CBSA' should start with these headers: "ZIP,CBSA,RES_RATIO,BUS_RATIO,OTH_RATIO,TOT_RATIO"
-The 'CBSA to MSA' should start with these headers: "CBSA,MDIV,STCOU,NAME,LSAD" and can have "POPESTIMATE2014" and "POPESTIMATE2015" in any order.

At this time, there is no built-in authentication. 
Documentation is provided using Swagger. At this time, the API base/root url is hardcoded into the SwaggerConfig.cs file
