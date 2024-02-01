'use client'

import axiosService from "@/services/axiosService";
import OnlineAuction from "@/components/Dashboard/OnlineAuction";
import Breadcrumb from "@/components/Home/Breadcrumb";
import CategoryCard from "@/components/Home/CategoryCard";
import { category1, category2, categoryItem1, categoryItem2 } from "@/data/item";
import { Category } from "@/types/models/category";
import axios from "axios";
import { useEffect, useState } from "react";
import https from 'https'

export default function Home() {
  const [categoryData, setCategoryData] = useState<Category[]>([]);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const res = await axios.get(process.env.NEXT_PUBLIC_SERVER_URL+"/api/user/CategoriesWithTenItems", {
          httpsAgent: new https.Agent({ rejectUnauthorized: false }),
        });
        const data: Category[] = res.data;
        setCategoryData(data);
      } catch (error) {
        console.error('Error fetching categories:', error);
        // Handle the error or provide a fallback mechanism if needed
      }
    };

    fetchData();
  }, []); // Run this effect only once on component mount

  return (
    <>
      <Breadcrumb listPages={[]} />
      <div className="space-y-20">
        {categoryData.map((cate) => (
          <CategoryCard key={cate.categoryId} category={cate} />
        ))}
      </div>
    </>
  );
}