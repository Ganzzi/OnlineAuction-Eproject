import Breadcrumb from "@/components/Home/Breadcrumb";
import { Category } from "@/types/models/category";
import { Item } from "@/types/models/item";
import { SearchParams } from "@/types/next";
import { category1, category2, item1 } from "@/data/item";
import Index from "./Index";
import axios from "axios";
import https from 'https'

// Fake data
const itemData = {...item1}
const categoriesData = [
  category1,
  category2,
  {
    categoryId: 305,
    categoryName: 'None',
    description: 'NOne',
  }
]
const existedCategories = [
  category1,
]

const getItemById: (itemId: number) => Promise<[Item, Category[]]> = async (itemId: number) => {
  const url = new URL(process.env.NEXT_PUBLIC_SERVER_URL+'/api/User/getItemById')
  url.searchParams.set("id", itemId.toString());

  const response = await axios.get(url.toString(), {
    httpsAgent: new https.Agent({ rejectUnauthorized: false }), // Ignore SSL certificate validation errors
  });

  const itemData: Item = response.data;
  
  return [itemData, itemData?.categoryItems?.map(ci => ci?.category) ?? []];
};

export const getListCategories: () => Promise<Category[]> = async () => {
  const url = new URL(process.env.NEXT_PUBLIC_SERVER_URL+'/api/User/CategoriesWithTenItems')

  const response = await axios.get(url.toString(), {
    httpsAgent: new https.Agent({ rejectUnauthorized: false }),
  });

  const categoriesData: Category[] = response.data;

  return categoriesData;
};

const ItemSellPage = async ({ searchParams }: { searchParams: SearchParams }) => {
  const itemId: number = parseInt(searchParams?.itemId as string);
  const itemData: [Item, Category[]] | undefined = itemId ?  await getItemById(itemId) : undefined;
  const categories = await getListCategories();

  return (
    <>
      <Breadcrumb listPages={[
    {
      pageName: "market",
      link: "/items/"
    },{
      pageName: `${!itemId ? "sell" : "update - "+itemId}` ,
      link: "/items/form"+ itemId && "?itemId="+itemId
    },
    ]} />
      <Index item={itemData?.[0]} categories={categories} existedCategories={itemData?.[1]} />
    </>
  );
};

export default ItemSellPage;
