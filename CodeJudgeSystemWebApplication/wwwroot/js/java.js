async function fetchAndDisplayPDFs() {
    const pdfUrls = [
        "./Pdf-files/lecture-1.pdf",
        "./Pdf-files/lecture-2.pdf",
        "./Pdf-files/lecture-3.pdf",
        "./Pdf-files/lecture-4.pdf",
        "./Pdf-files/lecture-5.pdf"
    ];

    const container = document.createElement('div');
    document.body.appendChild(container);

    for (let i = 0; i < pdfUrls.length; i++) {
        const pdfUrl = pdfUrls[i];

        try {
            const response = await fetch(pdfUrl);
            const blob = await response.blob();

            const lectureTitle = `Lecture ${i + 1}`;
            const iframe = document.createElement('iframe');
            iframe.src = URL.createObjectURL(blob);
            iframe.style.width = '35%';
            iframe.style.height = '500px';

            const paragraph = document.createElement('p');
            paragraph.className = 'lectures';
            paragraph.textContent = lectureTitle;

            container.appendChild(paragraph);
            container.appendChild(iframe);
        } catch (error) {
            console.error(`Failed to fetch PDF from ${pdfUrl}: ${error.message}`);
        }
    }
}

// Call the function to fetch and display PDFs when the script is executed
fetchAndDisplayPDFs();
