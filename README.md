# umbraco-demo
demonstration project

#important urls
http://demo.umbraco.com/umbraco#/login/false?returnPath=%252Fumbraco

## credentials
username: test@test.test
password: testtest

# Umbraco backoffice settings
I'm not able now to synchronize *Culture and hostname* for nodes in Content section. So this step needs to be performed manually.
If someone knows how to do it, please let me know :)

To correctly set up the hostnames and culture, do following steps:
+ Go to backoffice
+ Right click on *English Version* node
+ Left click on *Culture and Hostnames*
+ Set **Language** field to have value _en-US_
+ Click on **Add new domain** button
+ In **Domain** field insert _demo.umbraco.com_
+ In **Language** select _en-US_
+ Click **Save** button at the bottom of menu

Repeat the same steps for *Polish Version* node. Instead of _en-US_ culture, set it to _pl-PL_. Instead _demo.umbraco.com_ domain, set it to _demo.umbraco.com/pl_.
