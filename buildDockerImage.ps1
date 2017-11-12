param([string]$buildVersion, [string]$currentBranch, [string]$username, [string]$secret)

#if($currentBranch -eq 'master') {
	Write-Host "building Docker image"
	Set-Location .\src\Contentment.Api
	docker build -t contentmentcms/contentment.api:$($buildVersion).beta .

	Write-Host "pushing Docker image"
	docker login -u="$($username)" -p="$($secret)"
	docker push contentmentcms/contentment.api:$($buildVersion).beta

	# reset location
	Set-Location ../..
#}