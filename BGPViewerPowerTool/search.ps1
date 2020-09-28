Import-Module .\api_module.psm1
PrintInitialMessage
Write-Host ""
# Call the API
$model = RetrieveJsonFrom("https://api.bgpview.io/search?query_term=$($args[0])")
Write-Host "=== Search by $($args[0]) ==="
Write-Host ""
$asnSummary = ""
foreach ($asn in $model.data.asns) {
    $asnSummary += "$($asn.asn) "
    Write-Host "AS Number & Description: $($asn.asn)  $($asn.name) $($asn.description)"
    Write-Host "Country Code: $($asn.country_code)"
    Write-Host "Email Contacts: $($asn.email_contacts)"
    Write-Host "Abuse Contacts: $($asn.abuse_contacts)"
    Write-Host ""
}
Write-Host "# IPv4 Prefixes"
Write-Host ""
foreach ($prefix in $model.data.ipv4_prefixes) {
    Write-Host "Prefix: $($prefix.prefix)"
    Write-Host "Description: $($prefix.name), $($prefix.description)"
    Write-Host "Parent Prefix: $($prefix.parent_prefix)"
    Write-Host ""
}
Write-Host "# IPv6 Prefixes"
Write-Host ""
foreach ($prefix in $model.data.ipv6_prefixes) {
    Write-Host "Prefix: $($prefix.prefix)"
    Write-Host "Description: $($prefix.name), $($prefix.description)"
    Write-Host "Parent Prefix: $($prefix.parent_prefix)"
    Write-Host ""
}
Write-Host "=== Search by $($args[0]) summary ==="
Write-Host ""
Write-Host "Related ASNs: $($asnSummary.ToString())"
Write-Host ""