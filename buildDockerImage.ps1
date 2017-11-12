param([string]$buildVersion, [string]$currentBranch, [string]$username, [string]$password)

#if($currentBranch -eq 'master') {
	Write-Host "building Docker image"
	cd .\src\Contentment.Api
	docker build -t contentmentcms/contentment.api:$($buildVersion).beta .

	Write-Host "pushing Docker image"
	docker login -u=$($username) -p=$($password)
	docker push contentmentcms/contentment.api:$($buildVersion).beta
#}