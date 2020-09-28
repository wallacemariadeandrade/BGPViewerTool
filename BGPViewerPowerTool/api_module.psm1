# Function to retrieve json data from api 
function RetrieveJsonFrom {
    param (
        $ApiUrl
    )
    return ConvertFrom-Json(Invoke-WebRequest($ApiUrl))
}

function PrintInitialMessage () {
    Write-Host ""
    Write-Host "*** Powered by BGPView API ***"   
    Write-Host "*** Available at https://github.com/wallacemariadeandrade/BGPViewerTool.git ***"
}