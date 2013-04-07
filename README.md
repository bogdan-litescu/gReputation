gReputation
===========

Implemented at SuperHappyDevHouse Singapore 3.0, mostly as research on our reputation system for [OpenTraits](http://opentraits.com).
The service in implemented in Asp.NET MVC4 (C#) to be hosted in the Azure Cloud.
The UI is built with jQuery, Twitter Boostrap and AngularJs.

App is live at [greputation.cloudapp.net](http://greputation.cloudapp.net/).

Usage instructions:
*    Go to http://greputation.cloudapp.net/ and create a new app by providing a name and an email address. Note down appName and appKey.

*    Go to http://greputation.cloudapp.net/admin and add some rule. For example, say that when something/someone (so th object) is "liked" it should get +5 to quality. Note that you could do this with the REST Api (see page source)

*    Go to http://greputation.cloudapp.net/text and trigger an action. For example, say "User1" "liked" on "User2". The action name must be exactly as in previous step. Note that you could do this with the REST Api (see page source)

*    On the same page, use the form on the right to get the reputation of User2. It should be 5.