sudo rm -r sample/Documentation/* Documentation/*
sudo docker buildx build .
cd sample
sudo docker run -v $(pwd):/data -it $(sudo docker images | awk '{print $3}' | awk 'NR==2') doxygen
cd ..
mkdir -p Documentation
sudo cp -r sample/Documentation/html/* Documentation
cd Documentation