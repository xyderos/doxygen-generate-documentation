FROM ubuntu:22.04

RUN apt-get update && apt-get install -y sudo graphviz git cmake build-essential python3 flex bison
RUN adduser --disabled-password --gecos '' docker
RUN adduser docker sudo
RUN echo '%sudo ALL=(ALL) NOPASSWD:ALL' >> /etc/sudoers
RUN git clone https://github.com/doxygen/doxygen.git
WORKDIR /doxygen
RUN mkdir build
WORKDIR /doxygen/build
RUN cmake -G "Unix Makefiles" ..
RUN make
RUN make install
WORKDIR /data
VOLUME ["/data"]
WORKDIR /data