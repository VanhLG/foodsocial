const form = document.getElementById('post-form');
const postTypeInput = document.getElementById('post-type');
const contentInput = document.getElementById('post-content');
const imageInput = document.getElementById('image-upload');
const imageContainer = document.getElementById('image-container');


imageInput.addEventListener('change', handleImageUpload);
                
function handleImageUpload(event) {
    const imageFile = event.target.files[0];
    if (imageFile) {
        const imageURL = URL.createObjectURL(imageFile);
        const image = document.createElement('img');
        image.src = imageURL;
        imageContainer.innerHTML = '';
        imageContainer.appendChild(image);
        imageContainer.classList.remove('hide_img');
    }
}

// xử lý theo post
document.getElementById('btn_create').addEventListener('click', function (e) {
    e.preventDefault();
    
    const imageFile = document.getElementById('image-upload').files[0];
    const postType = document.getElementById('post-type').value;
    const postContent = document.getElementById('post-content').value;
    const countPost = document.getElementsByClassName('post').length;
    console.log(postType);
    const formData = new FormData();
    formData.append('image', imageFile);
    formData.append('postType', postType);
    formData.append('postContent', postContent);

    console.log(imageFile, postType, postContent);

    fetch('/TaoBaiViet/CratePost', {
        method: 'POST',
        body: formData
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                // Tải lại trang hiện tại
                window.location.reload();
            } else {
                console.error('Failed to create post');
            }
        })
        .catch(error => console.error('Error:', error));

    // sử lý thêm mới không tại lại trang nhưng chưa sử lý đc mã bài viết
    /*fetch('/TaoBaiViet/CratePost', {
        method: 'POST',
        body: formData
    })
        .then(response => {
            console.log('Response status:', response.statusText);
            return response.json();
        })
        .then(data => {
            console.log('Response data:', data);
            if (data.success) {
                // Tạo thẻ div mới
                const newDiv = document.createElement('div');
                console.log(data.us)
                // Tạo nội dung cho thẻ div mới
                let sdiv = `<div class="post" id="post-${countPost}">
                        <div class="info">
                            <div class="person">
                                <img src="${data.us.AnhDaiDien}">
                                <a>${data.us.TenNguoiDung}</a>
                                <span class="circle">.</span>
                                <span>${data.Date}</span>
                                <br />
                                <span style="margin-left: 35px; font-size: small; position: relative; top: -10px;">${postType}</span>
                            </div>
                            <div class="more" onmouseover="showMenu(${countPost})" onmouseout="hideMenu(${countPost})">
                                <img src="../LayoutTrangChu/img/show_more.png" alt="show more">
                                <div class="menu-content" id="menuContent_${countPost}">
                                    <button onclick="editItem(${countPost})">Sửa</button>
                                    <button onclick="deleteItem(${countPost})">Xóa</button>
                                </div>
                            </div>
                        </div>`;

                if (data.urlImg) {
                    sdiv += `<div class='image'><img src='${data.urlImg}' alt='Post Image'></div>`;
                }

                sdiv += `<div class="desc">
                        <div class="icons">
                            <div class="icon_left d-flex">
                                <div class="like">
                                    <img class="not_loved button_like" data-mabaiviet="${countPost}" data-tennguoidung="@baiViet.MaNguoiDung" src="../LayoutTrangChu/img/love.png">
                                    <img class="loved hide_img button_like" data-mabaiviet="${countPost}" data-tennguoidung="@baiViet.MaNguoiDung" src="../LayoutTrangChu/img/heart.png">
                                </div>
                            </div>
                        </div>
                        <div class="liked">
                            <a class="bold" id="like-count-${countPost}">@baiViet.SoLuongLike likes</a>
                        </div>
                        <div class="post_desc">
                            <p>
                                <a class="bold" href="#">${data.us.TenNguoiDung}</a> ${postContent}
                            </p>
                            <p> Comments </p>

                            <div class="comment-section" data-mabaiviet="${countPost}" data-tennguoidung="${data.us.MaNguoiDung}">
                                <input type="text" class="commentInput" placeholder="Add a comment..." />
                            </div>
                        </div>
                    </div>
                </div>`;

                newDiv.innerHTML = sdiv;

                // Lấy phần tử cha và phần tử con đầu tiên của nó
                const postsDiv = document.getElementById('posts');
                const firstChild = postsDiv.firstChild;

                console.log("newDiv", newDiv);
                console.log(postsDiv);

                // Sử dụng insertBefore để thêm phần tử mới
                postsDiv.insertBefore(newDiv, firstChild);
                $('#create_modal').modal('hide');
            }
        })
        .catch(error => {
            console.log(error);

        })
        .finally(() => console.log("Done!!!"));*/
});



function editForm(id) {
    console.log('Edit item called with ID:', id);

    // Gọi API để lấy dữ liệu của bài viết dựa trên ID
    fetch(`/TaoBaiViet/GetPost/${id}`)
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                console.log(data);

                // Nạp dữ liệu vào modal
                document.getElementById('post-type').value = data.postType;
                document.getElementById('post-content').value = data.postContent;

                // Nếu có ảnh, hiện ảnh đó
                const imageContainer = document.getElementById('image-container');
                if (data.imageUrl) {
                    imageContainer.innerHTML = `<img src="${data.imageUrl}" alt="Post Image" style="max-width: 100%; height: auto;">`;
                } else {
                    imageContainer.innerHTML = ''; // Nếu không có ảnh, xóa nội dung
                }

                // Xóa sự kiện cũ nếu có
                document.getElementById("btn_create").removeEventListener('click', handleEditPost);

                // Thêm sự kiện click mới cho nút
                document.getElementById("btn_create").addEventListener('click', handleEditPost);

                // Mở modal
                var myModal = new bootstrap.Modal(document.getElementById('create_modal'), {
                    keyboard: false
                });
                myModal.show();

                // Đặt ID vào một biến  toàn cầu để sử dụng trong handleEditPost
                window.currentPostId = id;
            } else {
                alert('Không thể tải dữ liệu bài viết.');
            }
        })
        .catch(error => console.error('Error:', error));
}

function handleEditPost() {
    const imageFile = document.getElementById('image-upload').files[0];
    const postType = document.getElementById('post-type').value;
    const postContent = document.getElementById('post-content').value;

    console.log(postType);
    const formData = new FormData();

    // Chỉ thêm file vào formData nếu có file được chọn
    if (imageFile) {
        formData.append('image', imageFile);
    }

    formData.append('postType', postType);
    formData.append('postContent', postContent);
    formData.append('id', window.currentPostId);

    console.log(imageFile, postType, postContent, window.currentPostId);

    fetch('/TaoBaiViet/EditPost', {
        method: 'POST',
        body: formData
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                // Tải lại trang hiện tại
                window.location.reload();
            } else {
                console.error('Failed to update post');
            }
        })
        .catch(error => console.error('Error:', error));
}

function deleteItem(id) {
    console.log('doDelete called with id:', id);

    // Xác nhận hành động xóa từ người dùng
    var ans = confirm('Bạn có chắc chắn muốn xóa không?');
    if (ans) {
        
        
        fetch('/TaoBaiViet/DeletePost/'+id, {
            method: 'GET'
        })
            .then(response => {
                console.log('Response status:', response.status);
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                return response.json();
            })
            .then(data => {
                console.log('Response data:', data);
                // Xóa hàng khỏi bảng sau khi xóa thành công
                if (data.success) {
                    document.getElementById('post-' + id).remove();
                   
                } else {
                    
                }
            })
            .catch(error => console.error('Error: ', error))
            .finally(() => console.log("Done!!!"));
    }
}


