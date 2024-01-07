// java-videos.js

async function fetchAndDisplayVideos() {
    const videoUrls = [
        "./Videos/java-video-1.mp4",
        "./Videos/java-video-2.mp4",
        "./Videos/java-video-3.mp4",
        "./Videos/java-video-4.mp4",
        "./Videos/java-video-5.mp4"
    ];

    const container = document.body;

    for (let i = 0; i < videoUrls.length; i++) {
        const videoUrl = videoUrls[i];

        try {
            const response = await fetch(videoUrl);
            const blob = await response.blob();

            const lectureTitle = `Lecture ${i + 1}`;
            const videoElement = document.createElement('video');
            videoElement.width = 640;
            videoElement.height = 360;
            videoElement.controls = true;

            const sourceElement = document.createElement('source');
            sourceElement.src = URL.createObjectURL(blob);
            sourceElement.type = 'video/mp4';

            videoElement.appendChild(sourceElement);

            const paragraph = document.createElement('p');
            paragraph.className = 'video-lectures';
            paragraph.textContent = lectureTitle;

            container.appendChild(paragraph);
            container.appendChild(videoElement);
        } catch (error) {
            console.error(`Failed to fetch video from ${videoUrl}: ${error.message}`);
        }
    }
}

// Call the function to fetch and display videos when the script is executed
fetchAndDisplayVideos();