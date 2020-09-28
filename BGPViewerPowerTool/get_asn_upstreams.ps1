Import-Module .\api_module.psm1
PrintInitialMessage
Write-Host ""
# Read ASN from input
$asn = $args[0]
# Call the API
$model = RetrieveJsonFrom("https://api.bgpview.io/asn/$($asn)/upstreams")
Write-Host "=== ASN$($asn) IPv4 Known Upstreams ==="
Write-Host ""
$ipv4_upstreams_list = ""
$ipv6_upstreams_list = ""
$ipv4_upstream_counter = 0
$ipv6_upstream_counter = 0
foreach ($upstream in $model.data.ipv4_upstreams) {
    $ipv4_upstreams_list += "$($upstream.asn) "
    Write-Host "ASN & Description: $($upstream.asn)  $($upstream.name) $($upstream.description)"
    Write-Host "Country Code: $($upstream.country_code)"
    Write-Host ""
    $ipv4_upstream_counter++
}
Write-Host ""
Write-Host "=== ASN$($asn) IPv6 Known Upstreams ==="
Write-Host ""
foreach ($upstream in $model.data.ipv6_upstreams) {
    $ipv6_upstreams_list += "$($upstream.asn) "
    Write-Host "ASN & Description: $($upstream.asn)  $($upstream.name) $($upstream.description)"
    Write-Host "Country Code: $($upstream.country_code)"
    Write-Host ""
    $ipv6_upstream_counter++
}
Write-Host ""
Write-Host "=== ASN$($asn) Known Upstreams Summary ==="
Write-Host ""
Write-Host "IPv4 Upstreams: $($ipv4_upstreams_list)"
Write-Host ""
Write-Host "IPv6 Upstreams: $($ipv6_upstreams_list)"
Write-Host ""
Write-Host "Total IPv4 Known Upstreams: $($ipv4_upstream_counter)"
Write-Host "Total IPv6 Known Upstreams: $($ipv6_upstream_counter)"
Write-Host ""