function PreviewFile() {
    const preview = document.getElementById('preview');
    const file = document.getElementById('fileInput').files[0];
    const reader = new FileReader();

    reader.onloadend = function () {
        preview.src = reader.result;
        preview.style.display = 'block';
    }

    if (file) {
        reader.readAsDataURL(file);
    } else {
        preview.src = "";
    }
    console.log("-preview-done-");
}