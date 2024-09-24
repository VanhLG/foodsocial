function showMenu(id) {
    document.getElementById("menuContent_"+id).style.display = "block";
}

function hideMenu(id) {
    document.getElementById("menuContent_"+id).style.display = "none";
}

// Close menu when clicking outside
window.onclick = function (event) {
    if (!event.target.matches('.more img')) {
        var dropdowns = document.getElementsByClassName("menu-content");
        for (var i = 0; i < dropdowns.length; i++) {
            var openDropdown = dropdowns[i];
            if (openDropdown.style.display === 'block') {
                openDropdown.style.display = 'none';
            }
        }
    }
}
function previewImage() {
    const file = document.getElementById('imageUpload').files[0];
    const imagePreview = document.getElementById('imagePreview');

    if (file) {
        const reader = new FileReader();

        reader.onload = function (e) {
            const img = document.createElement('img');
            img.src = e.target.result;
            img.classList.add('imgthumbnail');
            imagePreview.innerHTML = ''; // Clear any existing preview
            imagePreview.appendChild(img);
        };

        reader.readAsDataURL(file);
    }
}



/*function submitForm() {
    const form = document.getElementById('editProfileForm');
    
    const imgND = document.getElementById('imageUpload').files[0];
    const tenND = document.getElementById('tenND').value;
    const tieuSu = document.getElementById('tieuSu').value;
    
    const formData = new FormData();
    formData.append('image', imgND);
    formData.append('tenND', tenND);
    formData.append('TieuSu', tieuSu);
    console.log(formData);
    console.log(imgND);
    fetch('/TrangCaNhan/editProfile', {
        method: 'POST',
        body: formData
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok.');
            }
            return response.json();
        })
        .then(data => {
            if (data.success) {
                console.log('Profile updated successfully:', data);
                document.getElementById("tieu_Su").innerText = data.tieuS;
                document.getElementById("ten_ND").innerText = data.ten;
                if (data.urlImg != "") {
                    var AnhDD = document.getElementById("imgND");
                    const image = document.createElement('img');
                    AnhDD.innerHTML=""
                    image.src = data.urlImg;
                    AnhDD.appendChild(image);
                     
                }
                
                $('#editProfile').modal('hide'); 
            } else {
                console.error('Failed to update profile.');
            }
        })
        .catch(error => {
            console.error('There was a problem with the fetch operation:', error);
        });
}
  */