This project is the most common online shop, with role-based registration, product and order management.
To run the project, copy to Visual studio using the link, in powershell type update-database to create a local database, then you can run the project, and create an account with the admin role, than relog in. To change the role for the account you are creating in the path Areas/Identity/Pages/Account/Register.cshtml.cs at line 152 change the value from ‘Admin’ to ‘User’ on that's all there is to it.
Or you can see with this link http://sonseart-awf5c8c2e2f2bzde.eastus-01.azurewebsites.net
