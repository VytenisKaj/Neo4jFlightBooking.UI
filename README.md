Before lauching API you need Neo4j Docker image.

How to set up:
  1. Open Docker Desktop on your PC
  2. Open terminal, enter command ``` docker pull neo4j ``` 
  3. Enter command ``` docker images ```
  4. Find image with name "neo4j" and tag "latest" and copy its Image ID
  5. Enter command ``` docker run -p 7474:7474 -p 7687:7687 -d --name neo4j \
			-v $HOME/neo4j/data:/data -v $HOME/neo4j/logs:/logs \
			-v $HOME/neo4j/import:/var/lib/neo4j/import \
			-v $HOME/neo4j/plugins:/plugins \
			--env NEO4J_server_https_advertised__address="localhost:7473" \ 
			--env NEO4J_server_http_advertised__address="localhost:7474" \
			--env NEO4J_server_bolt_advertised__address="localhost:7687" \
			--env NEO4J_AUTH=none <Image ID> ```. Your Docker container should be working.
  6. To start/stop container at a later date, use ``` docker start neo4j ``` and ``` docker stop neo4j ``` respectively.
