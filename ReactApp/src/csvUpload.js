import React, { useState } from "react";
import { useDropzone } from "react-dropzone";
import axios from "axios";

// comment for test
const CsvUpload = () => {
  const [, setStatusMessage] = useState("");
  const [fileName, setFileName] = useState("");
  const [successfulUploads, setSuccessfulUploads] = useState(0);
  const [failedUploads, setFailedUploads] = useState(0);

  const onDrop = (acceptedFiles) => {
    const file = acceptedFiles[0];
    setFileName(file.name);
    handleUpload(file);
  };

  const handleUpload = async (file) => {
    const formData = new FormData();
    formData.append("file", file);

    try {
      const response = await axios.post(
        "https://localhost:7047/meter-reading-uploads",
        formData,
        {
          headers: {
            "Content-Type": "multipart/form-data",
          },
        }
      );

      const { successfulRecords, failedRecords } = response.data;
      setSuccessfulUploads(successfulRecords);
      setFailedUploads(failedRecords);
      setStatusMessage(
        `${successfulRecords} records were successfully uploaded, ${failedRecords} failed.`
      );
    } catch (error) {
      setStatusMessage("Error uploading the file. Please try again.");
      console.error(error);
    }
  };

  const { getRootProps, getInputProps } = useDropzone({
    accept: ".csv",
    onDrop,
  });

  return (
    <div style={{ padding: "20px", fontFamily: "Arial, sans-serif" }}>
      <h1 style={{ color: "#2c3e50", textAlign: "center" }}>
        Meter Reading Upload
      </h1>

      {fileName && (
        <p style={{ fontSize: "16px", color: "#34495e" }}>
          <strong>File selected:</strong> <b>{fileName}</b>
        </p>
      )}

      {successfulUploads > 0 && (
        <p style={{ fontSize: "16px", color: "#27ae60" }}>
          <strong>Successful uploads:</strong>{" "}
          <span style={{ fontWeight: "bold" }}>{successfulUploads}</span>
        </p>
      )}

      {failedUploads > 0 && (
        <p style={{ fontSize: "16px", color: "#e74c3c" }}>
          <strong>Failed uploads:</strong>{" "}
          <span style={{ fontWeight: "bold" }}>{failedUploads}</span>
        </p>
      )}

      <div
        {...getRootProps()}
        style={{
          border: "2px dashed #3498db",
          borderRadius: "8px",
          padding: "30px",
          textAlign: "center",
          cursor: "pointer",
          height: "250px",
          display: "flex",
          justifyContent: "center",
          alignItems: "center",
          backgroundColor: "#f4f6f7",
          boxShadow: "0 4px 8px rgba(0, 0, 0, 0.1)",
          transition: "all 0.3s ease-in-out",
          maxWidth: "1200px",
          margin: "auto",
        }}
      >
        <input {...getInputProps()} />
        <p
          style={{
            fontSize: "18px",
            color: "#7f8c8d",
            textAlign: "center",
            fontWeight: "bold",
          }}
        >
          Drag & drop a CSV file here, or click to select a file
        </p>
      </div>
    </div>
  );
};

export default CsvUpload;
