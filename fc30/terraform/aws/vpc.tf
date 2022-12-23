resource "aws_vpc" "new-vpc" {
    cidr_block = "10.0.0.0/16"
    tags = {
      "Name" = "${var.prefix}-vpc"
    }
}

data "aws_availability_zones" "available" {}

output "az" {
  value = "${data.aws_availability_zones.available.names}"
}

resource "aws_subnet" "subnets" {
  count = 2
  availability_zone = data.aws_availability_zones.available.names[count.index]
  vpc_id = aws_vpc.new-vpc.id
  cidr_block = "10.0.${count.index}.0/24"  
  map_public_ip_on_launch = true
  tags = {
    "Name" = "${var.prefix}-subnet-${count.index}"
  }
}

# resource "aws_subnet" "new-subnet-1" {
#   vpc_id = aws_vpc.new-vpc.id
#   cidr_block = "10.0.0.0/24"  
#   availability_zone = "us-east-1a"
#   tags = {
#     "Name" = "${var.prefix}-subnet-1"
#   }
# }

# resource "aws_subnet" "new-subnet-2" {
#   vpc_id = aws_vpc.new-vpc.id
#   cidr_block = "10.0.1.0/24"  
#   availability_zone = "us-east-1b"
#   tags = {
#     "Name" = "${var.prefix}-subnet-2"
#   }
# }