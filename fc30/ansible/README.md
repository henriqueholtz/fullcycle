# Ansible

By default Ansible connect with SSH.

https://github.com/devfullcycle/fc-ansible

### Run

- `docker-compose up -d`

### Configure keys between the control and nodes

- `docker exec -it node1 bash` and/or `docker exec -it node2 bash`

  - `service ssh start`

- `docker exec -it control bash`
  - `ssh-keygen` (generate a key without passphrase)
  - `ssh-copy-id root@node1` => send the public key to `root@node1` (require the root password)
  - `ssh root@node1` => login without password
  - `exit` (return back to the control)
  - `cd /root/asible`
  - `ansible -i hosts-docker all -m ping`

### Ansible Commands

- `ansible -i hosts-ping-local all -m ping`
- `ansible -i hosts-docker all -m ping`
- `ansible -i hosts-docker node1 -m ping`
- `ansible -i hosts-docker all -m apt -a "update_cache=yes name=git state=present"` (install `git` using `apt`)
- `ansible -i hosts-docker all -m apt -a "update_cache=yes name=git state=absent"` (remove `git` using `apt`)
- `ansible -i hosts-docker all -m git -a "repo=https://github.com/henriqueholtz/fullcycle dest=/root/repositories"`
- `ansible -i hosts-docker node1 -m setup`
- `ansible -i hosts-docker node1 -m shell -a "ls -la"`
- `ansible -i hosts-docker all -m apt -a "update_cache=yes name=nginx state=present"` (install `nginx` using `apt`)
- `ansible -i hosts-docker node1 -m shell -a "ps -aux"`

##### Ansible commands on AWS

- Create a free tier instances with `Amazon EC2 key pairs`
- [From my local shell] `chmod 400 /home/henriqueholtz/Projects/fullcycle/fc30/ansible/HENRIQUE-FC-AWS.pem ` To allow ansible access the file as ReadOnly
- PUT the path in inventory file (this case `hosts-aws`) in the `ansible_ssh_private_key_file` variable
- `ansible -i hosts-aws all -m ping`

### Ansible with Playbook

- `ansible-playbook -i hosts-aws playbook.yml`

### Ansible-Galaxy to organize playbooks

- `cd roles` -> `ansible-galaxy init install_ngnix`
