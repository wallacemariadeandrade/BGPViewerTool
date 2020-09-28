Import-Module .\api_module.psm1
PrintInitialMessage
Write-Host ""
# Read prefix from input
$ip = $args[0]
# Call the API
$model = RetrieveJsonFrom("https://api.bgpview.io/ip/$($ip)")
Write-Host "=== $($ip) Details ==="
Write-Host ""
Write-Host "IP: $($ip)"
Write-Host "RIR Allocation Prefix: $($model.data.rir_allocation.prefix)"
Write-Host "Country Code: $($model.data.rir_allocation.country_code)"
Write-Host "PTR Record: $($model.data.ptr_record)"
Write-Host "Prefixes:"
Write-Host ""
foreach ($prefix in $model.data.prefixes) {
    Write-Host "Prefix: $($prefix.prefix)"
    Write-Host "Name: $($prefix.name)"
    Write-Host "Description: $($prefix.description)"
    Write-Host "Related ASNs:"
    Write-Host ""
    foreach ($asn in $prefix.asn) {
        Write-Host "ASN: $($asn.asn)"
        Write-Host "Name: $($asn.name)"
        Write-Host "Description: $($asn.description)"
        Write-Host "Country Code: $($asn.country_code)"
        Write-Host ""
    }
    Write-Host ""
}