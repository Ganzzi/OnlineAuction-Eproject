import Breadcrumb from "@/components/Dashboard/Breadcrumb";
import TableOne from "@/components/common/Tables/TableOne";
import TableThree from "@/components/common/Tables/TableThree";
import TableTwo from "@/components/common/Tables/TableTwo";

import { Metadata } from "next";
export const metadata: Metadata = {
  title: "Tables Page | Next.js E-commerce Dashboard Template",
  description: "This is Tables page for BidHub Next.js",
  // other metadata
};

const TablesPage = () => {
  return (
    <>
      <Breadcrumb pageName="Tables" />

      <div className="flex flex-col gap-10">
        <TableOne />
        <TableTwo />
        <TableThree />
      </div>
    </>
  );
};

export default TablesPage;
