import Breadcrumb from "@/components/Home/Breadcrumb";
import { Category } from "@/types/models/category";
import { Item } from "@/types/models/item";
import { SearchParams } from "@/types/next";
import { category1, category2, item1 } from "@/data/item";
import Index from "./Index";

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
  // Fetch item details
  // const response = await fetch(`/api/items/${itemId}`);
  // const itemData = await response.json();

  return [itemData, existedCategories];
};

const getListCategories: () => Promise<Category[]> = async () => {
  // Fetch categories
  // const response = await fetch(``);
  // const categoriesData: Category[] = await response.json();

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
