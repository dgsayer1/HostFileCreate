# HostFileCreate
Create a hostfile text file by passing in servername(s). Useful when you don't know the specific sites or bindings on a server.

For example:

`HostFileCreate.exe SRV-WEB-01` will create one file called SRV-WEB-01 with all the sites and IPs or `HostFileCreate.exe SRV-WEB-01 SRV-WEB-02` will create two files with all the sites and IPs. The number of servers you can pass in at any one time is not limited but does increase the time to produce.


NOTE: You have to have access to those servers for this to be able to access the IIS Manager (to get the sites and bindings). This tool comes in use when the sites and bindings change on a regular basis.
