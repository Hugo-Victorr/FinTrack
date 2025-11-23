export const FintrackLogo = (props: React.ImgHTMLAttributes<HTMLImageElement>) => (
  <img
    src={"/fintrack_logo.png"}
    alt="FinTrack Logo"
    style={{ width: 24, height: 24, objectFit: "contain" }}
    {...props}
  />
);