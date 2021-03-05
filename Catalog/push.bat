docker build -f "c:\!code\asp.net\catalog\dockerfile" --force-rm -t catalog  --label "com.microsoft.created-by=visual-studio" --label "com.microsoft.visual-studio.project-name=Catalog" "c:\!code\asp.net"
docker tag catalog cernydocker/catalog:%1
docker push cernydocker/catalog:%1