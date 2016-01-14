# Pancake
This is Pancake. It lets you set up the desired state of your Persisted entities in a system. 

How to Use
There are three phases: Configuration, Resource Registration, and Serving

Configuration
Here you register Resource Providers and Behaviors.
Resource ordering will determine in what order Resources will be served.

Resource Registration
Here you declare what your system resources should look like.

Serving
This will apply all Resources to ensure their desired state.

Planned Supported Persistence Mechanisms (in order of implementation):
- [x] NHibernate 
- [ ] ADO.Net 
- [x] RavendDB - Maybe?
- [ ] Entity Framework?
