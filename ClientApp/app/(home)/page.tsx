import OnlineAuction from "@/components/Dashboard/OnlineAuction";
import Breadcrumb from "@/components/Home/Breadcrumb";
import CategoryCard from "@/components/Home/CategoryCard";
import { category1, category2, categoryItem1, categoryItem2 } from "@/data/item";
import { Category } from "@/types/models/category";
import { Metadata } from "next";

export const metadata: Metadata = {
  title: "TailAdmin | Next.js E-commerce Dashboard Template",
  description: "This is Home Blog page for TailAdmin Next.js",
  // other metadata
};

const categoryData: Category[] = [
  {
    ...category1, categoryItems: [
      categoryItem1, categoryItem2,
      categoryItem1, categoryItem2,
    ]
  },
  {
    ...category2, categoryItems: [
      categoryItem1, categoryItem2
    ]
  },
]

export default function Home() {
  return (
    <>
    <Breadcrumb listPages={[]} />
    <div className="space-y-20">
      {categoryData.map(cate => (
        <CategoryCard category={cate} />
        ))}
    </div>
        </>
  );
}
