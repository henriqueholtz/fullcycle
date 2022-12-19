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

output "data-source-result" {
  value = data.local_file.content-example.content
}

output "data-source-result-base64" {
  value = data.local_file.content-example.content_base64
}

output "my-content" {
  value = var.content
}

data "local_file" "content-example" {
  filename = "example.txt"
}