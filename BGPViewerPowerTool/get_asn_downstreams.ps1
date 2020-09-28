Import-Module .\api_module.psm1
PrintInitialMessage
Write-Host ""
# Read ASN from input
$asn = $args[0]
# Call the API
$model = RetrieveJsonFrom("https://api.bgpview.io/asn/$($asn)/downstreams")
Write-Host "=== ASN$($asn) IPv4 Known Downstreams ==="
Write-Host ""
$ipv4_downstreams_list = ""
$ipv6_downstreams_list = ""
$ipv4_downstream_counter = 0
$ipv6_downstream_counter = 0
foreach ($downstream in $model.data.ipv4_downstreams) {
    $ipv4_downstreams_list += "$($downstream.asn) "
    Write-Host "ASN & Description: $($downstream.asn)  $($downstream.name) $($downstream.description)"
    Write-Host "Country Code: $($downstream.country_code)"
    Write-Host ""
    $ipv4_downstream_counter++
}
Write-Host ""
Write-Host "=== ASN$($asn) IPv6 Known Downstreams ==="
Write-Host ""
foreach ($downstream in $model.data.ipv6_downstreams) {
    $ipv6_downstreams_list += "$($downstream.asn) "
    Write-Host "ASN & Description: $($downstream.asn)  $($downstream.name) $($downstream.description)"
    Write-Host "Country Code: $($downstream.country_code)"
    Write-Host ""
    $ipv6_downstream_counter++
}
Write-Host ""
Write-Host "=== ASN$($asn) Known Downstreams Summary ==="
Write-Host ""
Write-Host "IPv4 Downstreams: $($ipv4_downstreams_list)"
Write-Host ""
Write-Host "IPv6 Downstreams: $($ipv6_downstreams_list)"
Write-Host ""
Write-Host "Total IPv4 Known Downstreams: $($ipv4_downstream_counter)"
Write-Host "Total IPv6 Known Downstreams: $($ipv6_downstream_counter)"
Write-Host ""