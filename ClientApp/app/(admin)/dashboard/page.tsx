import OnlineAuction from "@/components/Dashboard/OnlineAuction";
import { Metadata } from "next";

export const metadata: Metadata = {
  title: "BidHub | Next.js E-commerce Dashboard Template",
  description: "This is Home Blog page for BidHub Next.js",
  // other metadata
};

export default function Home() {
  return (
    <>
      <OnlineAuction />
    </>
  );
}
