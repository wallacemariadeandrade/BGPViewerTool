Import-Module .\api_module.psm1
PrintInitialMessage
Write-Host ""
# Read ASN from input
$asn = $args[0]
# Call the API
$model = RetrieveJsonFrom("https://api.bgpview.io/asn/$($asn)")
Write-Host "=== ASN$($asn) Details ==="
Write-Host ""

Write-Host "AS Number: $($model.data.asn)"
Write-Host "Name: $($model.data.name)"
Write-Host "Short Description: $($model.data.description_short)"
Write-Host "Email Contacts: $($model.data.email_contacts)"
Write-Host "Abuse Contacts: $($model.data.abuse_contacts)"
Write-Host ""
Write-Host "Looking Glass: $($model.data.looking_glass)"
Write-Host "Country Code: $($model.data.rir_allocation.country_code)"
Write-Host ""