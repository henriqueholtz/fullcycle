resource "local_file" "example" {
  filename = "example.txt"
  content  = var.content
}

variable content {
  type = string
  default = "Default value"
}

output "id-from-file" {
  value = resource.local_file.example.id # from resource "localfile" above
}

output "content" {
  value = var.content
}