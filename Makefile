gd:
	python QuickScripts/build_gdd.py Gdd  Assets/Resources/Csv

ms:
	msbuild  /verbosity:quiet  Sod.sln

.PHONY:gd
