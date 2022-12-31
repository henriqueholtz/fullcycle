module "new-vpc" {
    source = "./modules/vpc"
    prefix = var.prefix
    vpc_cidr_block = var.vpc_cidr_block
}

module "eks" {
    source = "./modules/eks"
    vpc_id = module.new-vpc.vpc_id # from ./vpc/outputs.tf
    prefix = var.prefix
    cluster_name = var.cluster_name
    retention_in_days = var.retention_in_days
    subnet_ids = module.new-vpc.subnet_ids
    desired_size = var.desired_size # from ./vpc/outputs.tf
    max_size = var.max_size
    min_size = var.min_size
}