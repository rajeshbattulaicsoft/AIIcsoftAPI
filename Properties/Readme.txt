STEP 1 : Take Latest both API and Client
STEP 2 : Build both API and UI
STEP 3 : After successful of Build Please run below command
STEP 4 : dotnet publish -c Release -o ./publish
STEP 5 : After successful of above command it will generate publish folder in your project root path
STEP 6: Login into VM 10.10.0.64
STEP 7 : Go to IIS and stop the both UI and API sites
Step 8: Go to D drive then go to web Application
STEP 9 : Take back up of both UI and API projects
STEP 10 : Replace of Above published folders into respctive API and UI Project folders
STEP 11 : Finally go to IIS start the UI and API sites.