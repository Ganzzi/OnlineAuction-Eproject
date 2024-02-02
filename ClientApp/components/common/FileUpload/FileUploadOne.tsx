'use client'

import React, { ChangeEvent } from 'react';

interface FileUploadProps {
    onFileChange: (file: File) => void;
    accept?: string
  }
  
  const FileUpload: React.FC<FileUploadProps> = ({ onFileChange, accept }) => {
    const handleFileChange = (e: ChangeEvent<HTMLInputElement>) => {
      const selectedFile = e.target.files && e.target.files[0];
  
      if (selectedFile) {
        // Handle the selected file as needed
        onFileChange(selectedFile);
      }
    };

  return (
    <div className="relative my-5.5 block w-full cursor-pointer appearance-none rounded border-2 border-dashed border-primary bg-gray py-4 px-4 dark:bg-meta-4 sm:py-7.5">
      <input
        type="file"
        accept={`${accept ? accept : "*"}`}
        className="absolute inset-0 z-50 m-0 h-full w-full cursor-pointer p-0 opacity-0 outline-none"
        onChange={handleFileChange}
      />
      <div className="flex flex-col items-center justify-center space-y-3">
        <span className="flex h-10 w-10 items-center justify-center rounded-full border border-stroke bg-white dark:border-strokedark dark:bg-boxdark">
          <svg
            width="16"
            height="16"
            viewBox="0 0 16 16"
            fill="none"
            xmlns="http://www.w3.org/2000/svg"
          >
            {/* Your SVG path data */}
          </svg>
        </span>
        <p>
          <span className="text-primary">Click to upload</span> or drag and drop
        </p>
        <p className="mt-1.5">SVG, PNG, JPG or GIF</p>
        <p>(max, 800 X 800px)</p>
      </div>
    </div>
  );
};

export default FileUpload;
