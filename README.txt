Scaffold-DbContext  "Data Source=SQLPC\SQL22;Initial Catalog=ELRCBatteries_SMPL_IcSoft;Persist Security Info=True;User ID=xxxx;Password=xxxx;TrustServerCertificate=True; Connection TimeOut=60000;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models/SMIcsoftDataModels -Context SMDBContext -Tables Employee,AccessLevel,Invent_SR,Invent_SRMaterial -f



Deployment Process
------------------
Pull Latest Code
    Retrieve the latest version of both the UI and API from the repository.

Build the Application
    Build both the API and UI components.

Generate Latest DLLs
    After a successful build, run the following command to generate the latest DLLs:
    dotnet publish -c Release -o ./publish

Stop Applications in IIS
    Navigate to IIS and stop both the UI and API applications.

Backup Existing DLLs
    Take a backup of the current DLLs in use.

Replace DLLs
    Replace the existing DLLs with the newly generated DLLs from step 3 and replace app settings which has taken in backup

Start Applications in IIS
    Go back to IIS and start both the UI and API applications.

Verify Deployment
    In IIS, click on the "Browse" button next to each application to verify they are running as expected.