FROM ubuntu:18.04

# Install Ansible
RUN apt-get update && apt-get install vim ssh ansible -y

# Change root password
RUN echo 'root:fullcycle' | chpasswd 

# Uncomment the "PermitRootLogin"
RUN sed -i 's/#PermitRootLogin prohibit-password/PermitRootLogin yes/' /etc/ssh/sshd_config

# Enable port 22
RUN sed -i 's/#Port 22/Port 22/' /etc/ssh/sshd_config

# Uncomment the "PubkeyAuthentication"
RUN sed -i 's/#PubkeyAuthentication/PubkeyAuthentication/' /etc/ssh/sshd_config

# Uncomment the "PasswordAuthentication"
RUN sed -i 's/#PasswordAuthentication/PasswordAuthentication/' /etc/ssh/sshd_config