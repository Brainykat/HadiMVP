# Introduction
Uses Microservices architecture with **DDD** approach. Written in .net 6 C#
## Customer Service
  Post request to API end point 
  Two events are raised to event bus service
1. New Identity Server User event
    This is consumed by identity server 4 Admin service to create a new system user with role "Business Owner" and claims eg (business Id)
2. New Customer Accounts
    This is consumed by Accounts Service to open new accounts as per the chart of accounts
### Adding Employee / System User
  Post request to API end point 
  One events are raised to event bus service
1. New Identity Server User event
    This is consumed by identity server 4 Admin service to create a new system user with role "Employee" and claims eg (business Id and Depertement head if one)
# Ordering Service
## New Order
  Post request to API end point 
- Save order to postgress DB
- Raise event to Government API
    This event will be consumed by infrastructure API which will update the record after receiving response from the external concern 
- Log to Mongo DB for credit Scoring
# Infrastructure Service
- On startup an Event Bus Consumer service is registered which listens to new Order events raised by ordering service and makes http POST request to Government API to log data for tax purposes  
# Running via Docker
- Project contains the `docker-compose.vs.debug.yml` and `docker-compose.override.yml` to enable debugging with a seeded environment.
- The following possibility to get a running seeded and debug-able (in VS) environment:
>   `docker-compose build`
>   `docker-compose up -d`
