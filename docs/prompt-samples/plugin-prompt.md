Create a new Dataverse plugin class for the account entity's pre-operation create 
message that validates the account name is not empty and sets the account number 
prefix to "ACC-" followed by today's date.