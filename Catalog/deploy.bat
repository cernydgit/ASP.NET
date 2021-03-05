rem kubectl create deployment catalog --image=registry.hub.docker.com/cernydocker/catalog  
rem kubectl expose deployment catalog --type="NodePort" --port 80
rem kubectl scale deployment catalog --replicas=2

kubectl set image deployments/catalog catalog=registry.hub.docker.com/cernydocker/catalog:%1  
curl http://localhost:30666/api/Items
kubectl logs deployment/catalog