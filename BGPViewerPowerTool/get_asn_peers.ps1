Import-Module .\api_module.psm1
PrintInitialMessage
Write-Host ""
# Read ASN from input
$asn = $args[0]
# Call the API
$model = RetrieveJsonFrom("https://api.bgpview.io/asn/$($asn)/peers")
Write-Host "=== ASN$($asn) IPv4 Known Peers ==="
Write-Host ""
$ipv4_peers_counter = 0
$ipv4_peers_list = ""
$ipv6_peers_list = ""
foreach ($peer in $model.data.ipv4_peers) {
    $ipv4_peers_list += "$($peer.asn) "
    Write-Host "ASN & Description: $($peer.asn)  $($peer.name) $($peer.description)"
    Write-Host "Country Code: $($peer.country_code)"
    Write-Host ""
    $ipv4_peers_counter++
}
Write-Host ""
Write-Host "=== ASN$($asn) IPv6 Known Peers ==="
Write-Host ""
$ipv6_peers_counter = 0
foreach ($peer in $model.data.ipv6_peers) {
    $ipv6_peers_list += "$($peer.asn) "
    Write-Host "ASN & Description: $($peer.asn)  $($peer.name) $($peer.description)"
    Write-Host "Country Code: $($peer.country_code)"
    Write-Host ""
    $ipv6_peers_counter++
}
Write-Host "=== ASN$($asn) Known Peers Summary ==="
Write-Host ""
Write-Host "IPv4 ASN Peers: $($ipv4_peers_list)"
Write-Host ""
Write-Host "IPv6 ASN Peers: $($ipv6_peers_list)"
Write-Host ""
Write-Host "Total IPv4 Known Peers: $($ipv4_peers_counter)"
Write-Host "Total IPv6 Known Peers: $($ipv6_peers_counter)"
Write-Host ""