Import-Module .\api_module.psm1
PrintInitialMessage
Write-Host ""
Write-Host ">>> For better results use the most general prefix <<<"
Write-Host ""
# Read prefix from input
$prefix = $args[0]
$cidr = $args[1]
# Call the API
$model = RetrieveJsonFrom("https://api.bgpview.io/prefix/$($prefix)/$($cidr)")
Write-Host "=== $($prefix)/$($cidr) Details ==="
Write-Host ""
foreach ($data in $model.data) {
    Write-Host "Prefix: $($data.prefix)"
    Write-Host "IP: $($data.ip)"
    Write-Host "CIDR: $($data.cidr)"
    Write-Host "ASN info: $($data.asns.asn) $($name) $($data.description_short)"
    Write-Host "Known Upstreams ASNs:"
    Write-Host ""
    foreach ($upstream_asn in $data.asns.prefix_upstreams) {
        Write-Host "ASN: $($upstream_asn.asn)"
        Write-Host "Name: $($upstream_asn.name)"
        Write-Host "Description: $($upstream_asn.description)"
        Write-Host "Country Code: $($upstream_asn.country_code)"
        Write-Host ""
    }
}