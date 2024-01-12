import Breadcrumb from "@/components/Dashboard/Breadcrumb";
import ChartFour from "@/components/common/Charts/ChartFour";
import ChartOne from "@/components/common/Charts/ChartOne";
import ChartThree from "@/components/common/Charts/ChartThree";
import ChartTwo from "@/components/common/Charts/ChartTwo";
import { Metadata } from "next";
export const metadata: Metadata = {
  title: "Chart Page | Next.js E-commerce Dashboard Template",
  description: "This is Chart Page for TailAdmin Next.js",
  // other metadata
};

const Chart = () => {
  return (
    <>
      <Breadcrumb pageName="Chart" />

      <div className="grid grid-cols-12 gap-4 md:gap-6 2xl:gap-7.5">
        <div className="col-span-12">
          <ChartFour />
        </div>
        <ChartOne />
        <ChartTwo />
        <ChartThree />
      </div>
    </>
  );
};

export default Chart;
