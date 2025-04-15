const PROXY_CONFIG = [
  {
    context: [
      "/employeeprojects",
      "/import",
    ],
    target: "http://localhost:5095", // 👈 Your .NET 9 API port here
    secure: false,                    // Disable SSL checks for self-signed dev certs
    changeOrigin: true,
    logLevel: "debug"
  }
]

module.exports = PROXY_CONFIG;
